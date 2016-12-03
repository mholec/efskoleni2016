CREATE PROCEDURE [dbo].GetAuthorsByLastName
    @LastName nvarchar = null
AS
BEGIN
    SET NOCOUNT ON;

	select * from authors where authors.Lastname = @LastName
END



CREATE VIEW [dbo].[AuthorsView]
AS
SELECT        dbo.Authors.*
FROM            dbo.Authors

GO
