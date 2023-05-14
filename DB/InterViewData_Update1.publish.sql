/*
InterViewData 的部署指令碼

這段程式碼由工具產生。
變更這個檔案可能導致不正確的行為，而且如果重新產生程式碼，
變更將會遺失。
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "InterViewData"
:setvar DefaultFilePrefix "InterViewData"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
偵測 SQLCMD 模式，如果不支援 SQLCMD 模式，則停用指令碼執行。
若要在啟用 SQLCMD 模式後重新啟用指令碼，請執行以下:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'必須啟用 SQLCMD 模式才能成功執行此指令碼。';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'正在建立 資料表 [dbo].[VoteItem]...';


GO
CREATE TABLE [dbo].[VoteItem] (
    [Sn]   INT           IDENTITY (1, 1) NOT NULL,
    [Item] NVARCHAR (50) NOT NULL
);


GO
PRINT N'正在建立 資料表 [dbo].[VoteRecord]...';


GO
CREATE TABLE [dbo].[VoteRecord] (
    [Voter]    NVARCHAR (50) NOT NULL,
    [VoteItem] INT           NOT NULL
);


GO
PRINT N'正在建立 程序 [dbo].[CheckVoteItemExist]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CheckVoteItemExist]
	@Item nvarchar(50)
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		select  Sn
		from VoteItem
		where @Item = @Item
	COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
END
GO
PRINT N'正在建立 程序 [dbo].[CheckVoteRecordExist]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CheckVoteRecordExist]
	@Voter nvarchar(50),
	@VoteItem int
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		select  VoteItem
		from VoteRecord
		where Voter = @Voter and VoteItem = @VoteItem
	COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
	
END
GO
PRINT N'正在建立 程序 [dbo].[DeleteVoteItem]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteVoteItem]
   @Sn int
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		Delete InterviewTest.dbo.VoteItem 
		where Sn = @Sn
		COMMIT;
	END TRY
    BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
END
GO
PRINT N'正在建立 程序 [dbo].[GetVoteItem]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetVoteItem]
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		select Sn,Item
		from InterviewTest.dbo.VoteItem
		COMMIT;
	END TRY
    BEGIN CATCH
        ROLLBACK;
    END CATCH;

	SET NOCOUNT ON;
END
GO
PRINT N'正在建立 程序 [dbo].[GetVoteItemItemBySn]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetVoteItemItemBySn]
   @Sn int
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		select Sn,Item 
		from InterviewTest.dbo.VoteItem
		where Sn = @Sn
		COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
END
GO
PRINT N'正在建立 程序 [dbo].[GetVoteItemSnByItem]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetVoteItemSnByItem]
   @Item nvarchar(50)
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
	select Sn
		from InterviewTest.dbo.VoteItem
		where Item = @Item
		COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
END
GO
PRINT N'正在建立 程序 [dbo].[GetVoteRecord]...';


GO
CREATE PROC [dbo].[GetVoteRecord]
as
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		Select Voter,VoteItem
		from VoteRecord
		order by Voter,VoteItem
		COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
END
GO
PRINT N'正在建立 程序 [dbo].[GetVoteRecordItemCount]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetVoteRecordItemCount]

AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		select Item,count(VoteItem) VoteItem
		  from [InterviewTest].[dbo].[VoteItem] left outer join [InterviewTest].[dbo].[VoteRecord] on Sn = VoteItem
		 group by Item,VoteItem
		 COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;

   
END
GO
PRINT N'正在建立 程序 [dbo].[SetVoteItem]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SetVoteItem]
   @Item nvarchar(50)
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		Insert into InterviewTest.dbo.VoteItem (Item)
		values (@Item)
		COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
END
GO
PRINT N'正在建立 程序 [dbo].[SetVoteRecord]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SetVoteRecord]
    @Parameter1 VARCHAR(50),
    @Parameter2 int
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		Insert into [InterviewTest].[dbo].[VoteRecord] (Voter,VoteItem)
		values (@Parameter1, @Parameter2)
		COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
END
GO
PRINT N'正在建立 程序 [dbo].[UpdateVoteItem]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateVoteItem]
   @Sn int,
   @Item nvarchar(50)
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		Update InterviewTest.dbo.VoteItem 
		set Item = @Item
		where Sn = @Sn
		COMMIT;
	END TRY
	BEGIN CATCH
        ROLLBACK;
    END CATCH;
	SET NOCOUNT ON;
END
GO
PRINT N'更新完成。';


GO
