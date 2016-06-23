CREATE TABLE [dbo].[AssessmentTemplate] (
    [TemplateID]      INT           IDENTITY (1, 1) NOT NULL,
    [TemplateVersion] INT           NOT NULL,
    [TemplateDate]    DATETIME2 (7) CONSTRAINT [DF_AssessmentTemplate_TemplateDate] DEFAULT (getdate()) NULL,
    [DefaultTemplate] BIT           NULL,
    CONSTRAINT [PK_AssessmentTemplate] PRIMARY KEY CLUSTERED ([TemplateID] ASC)
);

