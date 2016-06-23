CREATE TABLE [dbo].[Groups] (
    [GroupID]   INT          IDENTITY (1, 1) NOT NULL,
    [GroupName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED ([GroupID] ASC)
);

