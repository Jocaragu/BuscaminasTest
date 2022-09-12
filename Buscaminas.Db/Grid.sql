CREATE TABLE [dbo].[Board]
(
	[cell_id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [x_value] INT NOT NULL, 
    [y_value] INT NOT NULL, 
    [stepped_on] BIT NOT NULL,
    [mined] BIT NOT NULL, 
    [nearby_mines] INT NULL, 
    [flagged] BIT NULL
)
