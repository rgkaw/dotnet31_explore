CREATE PROCEDURE dbo.BuyBook  
  @Guid UniqueIdentifier,
  @Username nvarchar,
  @BookId int,
  @Identity nvarchar OUT  
AS  
INSERT INTO OwnedBook (Guid, Username, BookId) 
    VALUES(@Guid, @Username, @BookId)  
SET @Identity = SCOPE_IDENTITY()  
RETURN @@ROWCOUNT