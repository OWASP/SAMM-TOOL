CREATE TABLE [dbo].[Industry] (
    [IndustryID]    INT           IDENTITY (1, 1) NOT NULL,
    [IndustryName]  VARCHAR (100) NOT NULL,
    [IndustryOrder] INT           NOT NULL,
    CONSTRAINT [PK_Industry] PRIMARY KEY CLUSTERED ([IndustryID] ASC)
);

