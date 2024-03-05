USE [ShopDb]
GO
/****** Object:  Table [dbo].[BuyItems]    Script Date: 2024/3/5 下午 04:49:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuyItems](
	[IDNo] [int] IDENTITY(1,1) NOT NULL,
	[buyCount] [int] NULL,
	[Items_IDNo] [int] NOT NULL,
	[UserId] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BuyItemsPaid]    Script Date: 2024/3/5 下午 04:49:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuyItemsPaid](
	[IDNo] [int] IDENTITY(1,1) NOT NULL,
	[buyCount] [int] NOT NULL,
	[Items_IDNo] [int] NOT NULL,
	[UserId] [nvarchar](50) NOT NULL,
	[InsertDatetime] [datetime] NOT NULL,
	[Ships_UUID] [char](40) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 2024/3/5 下午 04:49:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[IDNo] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[count] [int] NULL,
	[src] [nvarchar](50) NULL,
	[price] [decimal](18, 0) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ships]    Script Date: 2024/3/5 下午 04:49:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ships](
	[IDNo] [int] IDENTITY(1,1) NOT NULL,
	[uuid] [char](40) NOT NULL,
	[Status] [int] NOT NULL,
	[ShipDatetime] [datetime] NULL,
	[ReceivedDatetime] [datetime] NULL,
	[UserId] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2024/3/5 下午 04:49:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[IDNo] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[Addr] [nvarchar](150) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Ships] ADD  CONSTRAINT [DF_資料表1_IsShip]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  StoredProcedure [dbo].[usp_ReadyPay]    Script Date: 2024/3/5 下午 04:49:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_ReadyPay]
	-- Add the parameters for the stored procedure here
	@UserId nvarchar(50)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--declare @UserId nvarchar(50) = '1';
    -- Insert statements for procedure here
	declare @now datetime = getdate();
	BEGIN TRY 
		BEGIN TRAN
		--鎖定
		update items set 
			items.count = items.count 
			from 
				items 
			inner join [BuyItems] on Items.IDNo = BuyItems.Items_IDNo 
					and BuyItems.UserId = @UserId;
		--確認庫存量是否足夠
		if (exists(
				select 
					1
				from 
					items 
				inner join [BuyItems] on Items.IDNo = BuyItems.Items_IDNo 
						and BuyItems.UserId = @UserId
				where items.count - BuyItems.buyCount < 0))
			begin
			select '庫存不足'
			return;
			end

		declare @Ships_UUID char(40) = newid();

		--寫入購買
		INSERT INTO [dbo].[BuyItemsPaid]
			   (
			   [buyCount]
			   ,[Items_IDNo]
			   ,[UserId]
			   ,[InsertDatetime]
			   ,Ships_UUID)
		select            
			   [buyCount]
			   ,[Items_IDNo]
			   ,[UserId] 
			   , @now as [InsertDatetime]
			   , @Ships_UUID
		 from 
			[dbo].[BuyItems]
		 where 1 =1 
			and [UserId] = @UserId;

		--新增出貨
		declare @待出貨 int = 0;

		INSERT INTO [dbo].[Ships]
				   ([uuid]
				   ,[Status]
				   ,[ShipDatetime]
				   ,[ReceivedDatetime]
				   ,UserId)
			 VALUES
				   (@Ships_UUID
				   ,@待出貨
				   ,@now
				   ,null
				   ,@UserId);

		--更新庫存
		update items set 
			items.count = items.count - BuyItems.buyCount 
			from 
				items 
			inner join [BuyItems] on Items.IDNo = BuyItems.Items_IDNo 
					and BuyItems.UserId = @UserId;
	
		--刪除暫存購買清單
		delete [BuyItems] where [UserId] = @UserId;
		select '';
		COMMIT TRAN
	END TRY
	BEGIN CATCH  
		ROLLBACK TRAN
		-- Execute error retrieval routine.  
		SELECT '[' + cast(ERROR_NUMBER() as char) + ']' + ERROR_MESSAGE(); 
	END CATCH;  

end
GO
