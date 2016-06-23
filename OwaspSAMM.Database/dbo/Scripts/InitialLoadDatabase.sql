USE [OwaspSAMMOS]
GO
SET IDENTITY_INSERT [dbo].[IndustryTargetScoreValues] ON 

GO
INSERT [dbo].[IndustryTargetScoreValues] ([ID], [Score]) VALUES (1, N'0')
GO
INSERT [dbo].[IndustryTargetScoreValues] ([ID], [Score]) VALUES (2, N'0+')
GO
INSERT [dbo].[IndustryTargetScoreValues] ([ID], [Score]) VALUES (3, N'1')
GO
INSERT [dbo].[IndustryTargetScoreValues] ([ID], [Score]) VALUES (4, N'1+')
GO
INSERT [dbo].[IndustryTargetScoreValues] ([ID], [Score]) VALUES (5, N'2')
GO
INSERT [dbo].[IndustryTargetScoreValues] ([ID], [Score]) VALUES (6, N'2+')
GO
INSERT [dbo].[IndustryTargetScoreValues] ([ID], [Score]) VALUES (7, N'3')
GO
SET IDENTITY_INSERT [dbo].[IndustryTargetScoreValues] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryColor]) VALUES (1, N'Governance', N'#00577C')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryColor]) VALUES (2, N'Construction', N'#803400')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryColor]) VALUES (3, N'Verification', N'#007F38')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryColor]) VALUES (4, N'Deployment', N'#840000')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Sections] ON 

GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (1, N'Strategy & Metrics')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (2, N'Policy & Compliance')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (3, N'Education & Guidance')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (4, N'Threat Assessment')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (5, N'Security Requirements')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (6, N'Security Architecture')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (7, N'Design Review')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (8, N'Code Review')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (9, N'Security Testing')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (10, N'Vulnerability Management')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (11, N'Environment Hardening')
GO
INSERT [dbo].[Sections] ([SectionID], [SectionName]) VALUES (12, N'Operational Enablement')
GO
SET IDENTITY_INSERT [dbo].[Sections] OFF
GO
SET IDENTITY_INSERT [dbo].[Industry] ON 

GO
INSERT [dbo].[Industry] ([IndustryID], [IndustryName], [IndustryOrder]) VALUES (1, N'Default Average', 1)
GO
INSERT [dbo].[Industry] ([IndustryID], [IndustryName], [IndustryOrder]) VALUES (2, N'Independent Software Vendor', 2)
GO
INSERT [dbo].[Industry] ([IndustryID], [IndustryName], [IndustryOrder]) VALUES (3, N'Online Service Provider', 3)
GO
INSERT [dbo].[Industry] ([IndustryID], [IndustryName], [IndustryOrder]) VALUES (4, N'Financial Services Organization', 4)
GO
INSERT [dbo].[Industry] ([IndustryID], [IndustryName], [IndustryOrder]) VALUES (5, N'Government Organization', 5)
GO
SET IDENTITY_INSERT [dbo].[Industry] OFF
GO
SET IDENTITY_INSERT [dbo].[IndustryTarget] ON 

GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (1, 1, 1, 1, N'0+')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (2, 1, 2, 1, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (3, 1, 3, 1, N'0+')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (4, 2, 4, 1, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (5, 2, 5, 1, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (7, 2, 6, 1, N'0+')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (8, 3, 7, 1, N'1+')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (9, 3, 8, 1, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (10, 3, 9, 1, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (11, 4, 10, 1, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (12, 4, 11, 1, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (13, 4, 12, 1, N'0+')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (14, 1, 1, 2, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (15, 1, 2, 2, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (16, 1, 3, 2, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (17, 2, 4, 2, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (18, 2, 5, 2, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (19, 2, 6, 2, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (20, 3, 7, 2, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (21, 3, 8, 2, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (22, 3, 9, 2, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (23, 4, 10, 2, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (24, 4, 11, 2, N'0')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (25, 4, 12, 2, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (26, 1, 1, 3, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (27, 1, 2, 3, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (28, 1, 3, 3, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (29, 2, 4, 3, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (30, 2, 5, 3, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (31, 2, 6, 3, N'1')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (32, 3, 7, 3, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (33, 3, 8, 3, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (34, 3, 9, 3, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (35, 4, 10, 3, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (36, 4, 11, 3, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (37, 4, 12, 3, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (38, 1, 1, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (39, 1, 2, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (40, 1, 3, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (41, 2, 4, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (42, 2, 5, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (43, 2, 6, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (44, 3, 7, 4, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (45, 3, 8, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (46, 3, 9, 4, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (47, 4, 10, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (48, 4, 11, 4, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (49, 4, 12, 4, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (50, 1, 1, 5, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (51, 1, 2, 5, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (52, 1, 3, 5, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (53, 2, 4, 5, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (54, 2, 5, 5, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (55, 2, 6, 5, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (56, 3, 7, 5, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (57, 3, 8, 5, N'2')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (58, 3, 9, 5, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (59, 4, 10, 5, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (60, 4, 11, 5, N'3')
GO
INSERT [dbo].[IndustryTarget] ([ID], [CategoryID], [SectionID], [IndustryID], [Score]) VALUES (61, 4, 12, 5, N'3')
GO
SET IDENTITY_INSERT [dbo].[IndustryTarget] OFF
GO
SET IDENTITY_INSERT [dbo].[AssessmentTemplate] ON 

GO
INSERT [dbo].[AssessmentTemplate] ([TemplateID], [TemplateVersion], [TemplateDate], [DefaultTemplate]) VALUES (1, 1, GETDATE(), 1)
GO
SET IDENTITY_INSERT [dbo].[AssessmentTemplate] OFF
GO
SET IDENTITY_INSERT [dbo].[TemplateCategories] ON 

GO
INSERT [dbo].[TemplateCategories] ([CatID], [TemplateID], [CategoryID], [CategoryOrder]) VALUES (1, 1, 1, 1)
GO
INSERT [dbo].[TemplateCategories] ([CatID], [TemplateID], [CategoryID], [CategoryOrder]) VALUES (2, 1, 2, 2)
GO
INSERT [dbo].[TemplateCategories] ([CatID], [TemplateID], [CategoryID], [CategoryOrder]) VALUES (3, 1, 3, 3)
GO
INSERT [dbo].[TemplateCategories] ([CatID], [TemplateID], [CategoryID], [CategoryOrder]) VALUES (4, 1, 4, 4)
GO
SET IDENTITY_INSERT [dbo].[TemplateCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[TemplateSections] ON 

GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (1, 1, 1, 1)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (2, 2, 2, 1)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (3, 3, 3, 1)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (4, 4, 4, 2)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (5, 5, 5, 2)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (6, 6, 6, 2)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (7, 7, 7, 3)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (8, 8, 8, 3)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (9, 9, 9, 3)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (10, 10, 10, 4)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (11, 11, 11, 4)
GO
INSERT [dbo].[TemplateSections] ([SecID], [SectionID], [SectionOrder], [CategoryID]) VALUES (12, 12, 12, 4)
GO
SET IDENTITY_INSERT [dbo].[TemplateSections] OFF
GO
SET IDENTITY_INSERT [dbo].[Groups] ON 

GO
INSERT [dbo].[Groups] ([GroupID], [GroupName]) VALUES (1, N'Group 1')
GO
INSERT [dbo].[Groups] ([GroupID], [GroupName]) VALUES (2, N'Group 2')
GO
INSERT [dbo].[Groups] ([GroupID], [GroupName]) VALUES (3, N'Group 3')
GO
INSERT [dbo].[Groups] ([GroupID], [GroupName]) VALUES (4, N'Group 4')
GO
INSERT [dbo].[Groups] ([GroupID], [GroupName]) VALUES (5, N'Group 5')
GO
INSERT [dbo].[Groups] ([GroupID], [GroupName]) VALUES (6, N'Group 6')
GO
SET IDENTITY_INSERT [dbo].[Groups] OFF
GO
SET IDENTITY_INSERT [dbo].[TemplateGroups] ON 

GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (1, 1, 1, 1)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (2, 2, 2, 1)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (3, 3, 3, 1)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (4, 1, 1, 2)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (5, 2, 2, 2)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (6, 3, 3, 2)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (7, 1, 1, 3)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (8, 2, 2, 3)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (9, 3, 3, 3)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (10, 1, 1, 4)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (11, 2, 2, 4)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (12, 3, 3, 4)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (13, 1, 1, 5)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (14, 2, 2, 5)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (15, 3, 3, 5)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (16, 1, 1, 6)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (17, 2, 2, 6)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (18, 3, 3, 6)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (19, 1, 1, 7)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (20, 2, 2, 7)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (21, 3, 3, 7)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (22, 1, 1, 8)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (23, 2, 2, 8)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (24, 3, 3, 8)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (25, 1, 1, 9)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (26, 2, 2, 9)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (27, 3, 3, 9)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (28, 1, 1, 10)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (29, 2, 2, 10)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (30, 3, 3, 10)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (31, 1, 1, 11)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (32, 2, 2, 11)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (33, 3, 3, 11)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (34, 1, 1, 12)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (35, 2, 2, 12)
GO
INSERT [dbo].[TemplateGroups] ([GroID], [GroupID], [GroupOrder], [SectionID]) VALUES (36, 3, 3, 12)
GO
SET IDENTITY_INSERT [dbo].[TemplateGroups] OFF
GO
SET IDENTITY_INSERT [dbo].[Questions] ON 

GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (1, N'Is there a software security assurance program already in place?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (2, N'Do most of the business stakeholders understand your organization’s risk profile?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (3, N'Is most of your development staff aware of future plans for the assurance program?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (4, N'Are most of your applications and resources categorized by risk?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (5, N'Are risk ratings used to tailor the required assurance activities?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (6, N'Does most of the organization know about what’s required based on risk ratings?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (7, N'Is per-project data for cost of assurance activities collected?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (8, N'Does your organization regularly compare your security spend with other organizations?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (9, N'Do most project stakeholders know their project’s compliance status?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (10, N'Are compliance requirements specifically considered by project teams?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (11, N'Does the organization utilize a set of policies and standards to control software development?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (12, N'Are project teams able to request an audit for compliance with policies and standards?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (13, N'Are projects periodically audited to ensure a baseline of compliance with policies and standards?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (14, N'Does the organization systematically use audits to collect and control compliance evidence?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (15, N'Have most developers been given highlevel security awareness training?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (16, N'Does each project team have access to secure development best practices and guidance?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (17, N'Are most roles in the development process given role-specific training and guidance?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (18, N'Are most stakeholders able to pull in security coaches for use on projects?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (19, N'Is security-related guidance centrally controlled and consistently distributed throughout the organization?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (20, N'Are most people tested to ensure a baseline skillset for secure development practices?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (22, N'Do most projects in your organization consider and document likely threats?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (23, N'Does your organization understand and document the types of attackers it faces?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (24, N'Do project teams regularly analyze functional requirements for likely abuses?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (25, N'Do project teams use a method of rating threats for relative comparison?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (26, N'Are stakeholders aware of relevant threats and ratings?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (27, N'Do project teams specifically consider risk from external software?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (28, N'Are all protection mechanisms and controls captured and mapped back to threats?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (29, N'Do most project teams specify some security requirements during development?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (30, N'Do project teams pull requirements from bestpractices and compliance guidance?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (31, N'Are most stakeholders reviewing access control matrices for relevant projects?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (32, N'Are project teams specifying requirements based on feedback from other security activities?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (33, N'Are most stakeholders reviewing vendor agreements for security requirements?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (34, N'Are the security requirements specified by project teams being audited?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (35, N'Are project teams provided with a list of recommended third-party components?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (36, N'Are most project teams aware of secure design principles and applying them?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (37, N'Do you advertise shared security services with guidance for project teams?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (38, N'Are project teams provided with prescriptive design patterns based on their application architecture?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (39, N'Are project teams building software from centrally controlled platforms and frameworks?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (40, N'Are project teams being audited for usage of secure architecture components?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (41, N'Do project teams document the attack perimeter of software designs?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (42, N'Do project teams check software designs against known security risks?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (43, N'Do most project teams specifically analyze design elements for security mechanisms?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (44, N'Are most project stakeholders aware of how to obtain a formal design review?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (45, N'Does the design review process incorporate detailed data-level analysis?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (46, N'Does routine project audit require a baseline for design review results?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (47, N'Do most project teams have review checklists based on common problems?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (48, N'Are project teams generally performing review of selected high-risk code?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (49, N'Can most project teams access automated code analysis tools to find security problems?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (50, N'Do most stakeholders consistently require and review results from code reviews?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (51, N'Do project teams utilize automation to check code against application-specific coding standards?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (52, N'Does routine project audit require a baseline for code review results prior to release?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (53, N'Are projects specifying some security tests based on requirements?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (54, N'Do most projects perform penetration tests prior to release?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (55, N'Are most stakeholders aware of the security test status prior to release?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (56, N'Are projects using automation to evaluate security test cases?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (57, N'Do most projects follow a consistent process to evaluate and report on security tests to stakeholders?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (58, N'Are security test cases comprehensively generated for application-specific logic?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (59, N'Do routine project audits demand minimum standard results from security testing?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (60, N'Do most projects have a point of contact for security issues?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (61, N'Does your organization have an assigned security response team?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (62, N'Are most project teams aware of their security point(s) of contact and response team(s)?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (63, N'Does the organization utilize a consistent process for incident reporting and handling?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (64, N'Are most project stakeholders aware of relevant security disclosures related to their software projects?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (65, N'Are most incidents inspected for root causes to generate further recommendations?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (66, N'Do most projects consistently collect and report data and metrics related to incidents?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (67, N'Do the majority of projects document some requirements for the operational environment?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (68, N'Do most projects check for security updates to third-party software components?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (69, N'Is a consistent process used to apply upgrades and patches to critical dependencies?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (70, N'Do most project leverage automation to check application and environment health?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (71, N'Are stakeholders aware of options for additional tools to protect software while running in operations?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (72, N'Does routine audit check most projects for baseline environment health?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (73, N'Do you deliver security notes with the majority of software releases?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (74, N'Are security-related alerts and error conditions documented for most projects?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (75, N'Are most project utilizing a change management process that’s well understood?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (76, N'Do project teams deliver an operational security guide with each product release?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (77, N'Are most projects being audited to check each release for appropriate operational security information?')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionText]) VALUES (78, N'Is code signing routinely performed on software components using a consistent process?')
GO
SET IDENTITY_INSERT [dbo].[Questions] OFF
GO
SET IDENTITY_INSERT [dbo].[TemplateQuestions] ON 

GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (1, 1, 1, 1)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (2, 2, 2, 1)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (3, 3, 3, 1)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (4, 4, 1, 2)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (5, 5, 2, 2)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (6, 6, 3, 2)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (7, 7, 1, 3)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (8, 8, 2, 3)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (9, 9, 1, 4)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (10, 10, 2, 4)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (11, 11, 1, 5)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (12, 12, 2, 5)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (13, 13, 1, 6)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (14, 14, 2, 6)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (15, 15, 1, 7)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (16, 16, 2, 7)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (17, 17, 1, 8)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (18, 18, 2, 8)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (19, 19, 1, 9)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (20, 20, 2, 9)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (21, 22, 1, 10)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (22, 23, 2, 10)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (23, 24, 1, 11)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (24, 25, 2, 11)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (25, 26, 3, 11)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (26, 27, 1, 12)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (27, 28, 2, 12)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (28, 29, 1, 13)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (29, 30, 2, 13)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (30, 31, 1, 14)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (31, 32, 2, 14)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (32, 33, 1, 15)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (33, 34, 2, 15)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (34, 35, 1, 16)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (35, 36, 2, 16)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (36, 37, 1, 17)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (37, 38, 2, 17)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (38, 39, 1, 18)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (39, 40, 2, 18)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (40, 41, 1, 19)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (41, 42, 2, 19)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (42, 43, 1, 20)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (43, 44, 2, 20)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (44, 45, 1, 21)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (45, 46, 2, 21)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (46, 47, 1, 22)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (47, 48, 2, 22)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (48, 49, 1, 23)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (49, 50, 2, 23)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (50, 51, 1, 24)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (51, 52, 2, 24)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (52, 53, 1, 25)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (53, 54, 2, 25)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (54, 55, 3, 25)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (55, 56, 1, 26)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (56, 57, 2, 26)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (57, 58, 1, 27)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (58, 59, 2, 27)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (59, 60, 1, 28)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (60, 61, 2, 28)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (61, 62, 3, 28)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (62, 63, 1, 29)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (63, 64, 2, 29)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (64, 65, 1, 30)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (65, 66, 2, 30)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (66, 67, 1, 31)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (67, 68, 2, 31)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (68, 69, 1, 32)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (69, 70, 2, 32)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (70, 71, 1, 33)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (71, 72, 2, 33)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (72, 73, 1, 34)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (73, 74, 2, 34)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (74, 75, 1, 35)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (75, 76, 2, 35)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (76, 77, 1, 36)
GO
INSERT [dbo].[TemplateQuestions] ([QueID], [QuestionID], [QuestionOrder], [GroupID]) VALUES (77, 78, 2, 36)
GO
SET IDENTITY_INSERT [dbo].[TemplateQuestions] OFF
GO
