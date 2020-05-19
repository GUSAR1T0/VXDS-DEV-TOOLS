CREATE TABLE [portal].[ActiveModuleConfiguration]
(
    [ModuleId]              INT NOT NULL,
    [ModuleConfigurationId] INT NOT NULL,
    CONSTRAINT [PK_ActiveModuleConfiguration_ModuleId] PRIMARY KEY CLUSTERED ([ModuleId] ASC),
    CONSTRAINT [FK_ActiveModuleConfiguration_ModuleId] FOREIGN KEY ([ModuleId]) REFERENCES [portal].[Module] ([Id]),
    CONSTRAINT [FK_ActiveModuleConfiguration_ModuleConfigurationId] FOREIGN KEY ([ModuleConfigurationId]) REFERENCES [portal].[ModuleConfiguration] ([Id]) ON DELETE CASCADE
);