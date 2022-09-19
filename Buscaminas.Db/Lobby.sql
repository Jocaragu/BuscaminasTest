CREATE TABLE [dbo].[Lobby]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [name] NVARCHAR(50) NOT NULL, 
    [highScore] INT NULL
)
