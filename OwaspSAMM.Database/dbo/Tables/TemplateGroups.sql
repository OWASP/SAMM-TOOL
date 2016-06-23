CREATE TABLE [dbo].[TemplateGroups] (
    [GroID]      INT IDENTITY (1, 1) NOT NULL,
    [GroupID]    INT NOT NULL,
    [GroupOrder] INT NOT NULL,
    [SectionID]  INT NOT NULL,
    CONSTRAINT [PK_TemplateGroups] PRIMARY KEY CLUSTERED ([GroID] ASC),
    CONSTRAINT [FK_TemplateGroups_Groups] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Groups] ([GroupID]),
    CONSTRAINT [FK_TemplateGroups_TemplateSections] FOREIGN KEY ([SectionID]) REFERENCES [dbo].[TemplateSections] ([SecID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID is primary key for this table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TemplateGroups', @level2type = N'COLUMN', @level2name = N'GroID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign key into Groups table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TemplateGroups', @level2type = N'COLUMN', @level2name = N'GroupID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Integer sort order for this group within the template', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TemplateGroups', @level2type = N'COLUMN', @level2name = N'GroupOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign key into Section table to where this group belongs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TemplateGroups', @level2type = N'COLUMN', @level2name = N'SectionID';

