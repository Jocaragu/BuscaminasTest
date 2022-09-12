CREATE TABLE [dbo].[Scoreboard]
(
	[game_id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [player_num] INT NOT NULL, 
    [player_name] NCHAR(10) NOT NULL,
    [score] INT NOT NULL, 
    [difficulty] NCHAR(10) NOT NULL, 
    [exploded] BIT NOT NULL
)
