CREATE TABLE [dbo].[Assessment] (
    [AssessmentID]     INT           IDENTITY (1, 1) NOT NULL,
    [TemplateVersion]  INT           NOT NULL,
    [OwnerID]          INT           NOT NULL,
    [OrganizationName] VARCHAR (100) NULL,
    [ApplicationName]  VARCHAR (50)  NULL,
    [LastUpdated]      DATETIME2 (7) NULL,
    [LastUpdateBy]     INT           NULL,
    [CreateDate]       DATETIME2 (7) NULL,
    [CreateBy]         INT           NULL,
    [IndustryID]       INT           CONSTRAINT [DF_Assessment_IndustryID] DEFAULT ((1)) NULL,
    [BusinessUnit]     VARCHAR (50)  NULL,
    [Finalized]        BIT           NULL,
    CONSTRAINT [PK_Assessment] PRIMARY KEY CLUSTERED ([AssessmentID] ASC),
    CONSTRAINT [FK_Assessment_Industry] FOREIGN KEY ([IndustryID]) REFERENCES [dbo].[Industry] ([IndustryID]),
    CONSTRAINT [FK_Assessment_UserData] FOREIGN KEY ([LastUpdateBy]) REFERENCES [dbo].[UserData] ([UserID]),
    CONSTRAINT [FK_Assessment_UserData1] FOREIGN KEY ([OwnerID]) REFERENCES [dbo].[UserData] ([UserID]),
    CONSTRAINT [FK_Assessment_UserData2] FOREIGN KEY ([CreateBy]) REFERENCES [dbo].[UserData] ([UserID])
);

