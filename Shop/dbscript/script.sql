USE [ShopDb]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ShipsStatusText]    Script Date: 2024/3/5 下午 06:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fn_ShipsStatusText]
(
	-- Add the parameters for the function here
	@status int
)
RETURNS nvarchar(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result nvarchar(20);

	-- Your existing logic
	IF @status = 0
		SET @result = N'待出貨';
	ELSE
		SET @result = N'未知狀態' + CAST(@status AS nvarchar(10));

	-- Ensure the RETURN is the last statement
	RETURN @result;
END
GO
/****** Object:  Table [dbo].[BuyItems]    Script Date: 2024/3/5 下午 06:25:52 ******/
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
/****** Object:  Table [dbo].[BuyItemsPaid]    Script Date: 2024/3/5 下午 06:25:52 ******/
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
/****** Object:  Table [dbo].[Items]    Script Date: 2024/3/5 下午 06:25:52 ******/
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
/****** Object:  Table [dbo].[Ships]    Script Date: 2024/3/5 下午 06:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ships](
	[IDNo] [int] IDENTITY(1,1) NOT NULL,
	[uuid] [char](40) NOT NULL,
	[Status] [int] NOT NULL,
	[PaidDatetime] [datetime] NULL,
	[ShipDatetime] [datetime] NULL,
	[ReceivedDatetime] [datetime] NULL,
	[UserId] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2024/3/5 下午 06:25:52 ******/
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
SET IDENTITY_INSERT [dbo].[BuyItemsPaid] ON 

INSERT [dbo].[BuyItemsPaid] ([IDNo], [buyCount], [Items_IDNo], [UserId], [InsertDatetime], [Ships_UUID]) VALUES (1, 3, 1, N'1', CAST(N'2024-03-05T16:41:50.120' AS DateTime), N'24B1C136-C43D-463F-B6EE-623D7209BC97    ')
INSERT [dbo].[BuyItemsPaid] ([IDNo], [buyCount], [Items_IDNo], [UserId], [InsertDatetime], [Ships_UUID]) VALUES (2, 4, 2, N'1', CAST(N'2024-03-05T16:41:50.120' AS DateTime), N'24B1C136-C43D-463F-B6EE-623D7209BC97    ')
INSERT [dbo].[BuyItemsPaid] ([IDNo], [buyCount], [Items_IDNo], [UserId], [InsertDatetime], [Ships_UUID]) VALUES (3, 2, 1, N'1', CAST(N'2024-03-05T16:44:45.147' AS DateTime), N'7473EC57-FDA2-4D26-870F-6B250197D250    ')
INSERT [dbo].[BuyItemsPaid] ([IDNo], [buyCount], [Items_IDNo], [UserId], [InsertDatetime], [Ships_UUID]) VALUES (4, 4, 2, N'1', CAST(N'2024-03-05T16:44:45.147' AS DateTime), N'7473EC57-FDA2-4D26-870F-6B250197D250    ')
INSERT [dbo].[BuyItemsPaid] ([IDNo], [buyCount], [Items_IDNo], [UserId], [InsertDatetime], [Ships_UUID]) VALUES (5, 3, 1, N'1', CAST(N'2024-03-05T16:48:04.180' AS DateTime), N'5C1CD5D6-1FCE-4010-AB7A-309C33923ABB    ')
INSERT [dbo].[BuyItemsPaid] ([IDNo], [buyCount], [Items_IDNo], [UserId], [InsertDatetime], [Ships_UUID]) VALUES (6, 4, 2, N'1', CAST(N'2024-03-05T16:48:04.180' AS DateTime), N'5C1CD5D6-1FCE-4010-AB7A-309C33923ABB    ')
SET IDENTITY_INSERT [dbo].[BuyItemsPaid] OFF
GO
SET IDENTITY_INSERT [dbo].[Items] ON 

INSERT [dbo].[Items] ([IDNo], [name], [count], [src], [price]) VALUES (1, N'商品1', 83, N'img/camera.jpg', CAST(123 AS Decimal(18, 0)))
INSERT [dbo].[Items] ([IDNo], [name], [count], [src], [price]) VALUES (2, N'商品2', 91, N'img/camera.jpg', CAST(33 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[Items] OFF
GO
SET IDENTITY_INSERT [dbo].[Ships] ON 

INSERT [dbo].[Ships] ([IDNo], [uuid], [Status], [PaidDatetime], [ShipDatetime], [ReceivedDatetime], [UserId]) VALUES (1, N'24B1C136-C43D-463F-B6EE-623D7209BC97    ', 0, CAST(N'2024-03-05T16:41:50.120' AS DateTime), NULL, NULL, N'1')
INSERT [dbo].[Ships] ([IDNo], [uuid], [Status], [PaidDatetime], [ShipDatetime], [ReceivedDatetime], [UserId]) VALUES (2, N'7473EC57-FDA2-4D26-870F-6B250197D250    ', 0, CAST(N'2024-03-05T16:44:45.147' AS DateTime), NULL, NULL, N'1')
INSERT [dbo].[Ships] ([IDNo], [uuid], [Status], [PaidDatetime], [ShipDatetime], [ReceivedDatetime], [UserId]) VALUES (3, N'988429AB-17EA-4FBE-9D84-28FB042CE3CD    ', 0, CAST(N'2024-03-05T16:44:45.147' AS DateTime), NULL, NULL, N'1')
INSERT [dbo].[Ships] ([IDNo], [uuid], [Status], [PaidDatetime], [ShipDatetime], [ReceivedDatetime], [UserId]) VALUES (4, N'5C1CD5D6-1FCE-4010-AB7A-309C33923ABB    ', 0, CAST(N'2024-03-05T16:48:04.180' AS DateTime), NULL, NULL, N'1')
SET IDENTITY_INSERT [dbo].[Ships] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([IDNo], [UserId], [UserName], [Addr]) VALUES (1, N'1', N'測試姓名', N'這是地址')
INSERT [dbo].[Users] ([IDNo], [UserId], [UserName], [Addr]) VALUES (2, N'2', N'測試姓名2', N'這是地址2')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Ships] ADD  CONSTRAINT [DF_資料表1_IsShip]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  StoredProcedure [dbo].[usp_ReadyPay]    Script Date: 2024/3/5 下午 06:25:52 ******/
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
				   ,PaidDatetime
				   ,UserId)
			 VALUES
				   (@Ships_UUID
				   ,@待出貨
				   ,@now
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
