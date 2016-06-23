CREATE TABLE [dbo].[Categories] (
    [CategoryID]    INT          IDENTITY (1, 1) NOT NULL,
    [CategoryName]  VARCHAR (50) NOT NULL,
    [CategoryColor] VARCHAR (50) NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);

