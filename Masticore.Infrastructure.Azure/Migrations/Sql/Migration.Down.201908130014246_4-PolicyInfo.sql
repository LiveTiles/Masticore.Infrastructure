If Object_Id('__MigrationHistory') Is Null 
Or Object_Id('TenantAlias') Is Null 
Or Not Exists(Select 1 From __MigrationHistory Where 1 = 1 And MigrationId = N'201908130014246_4-PolicyInfo')
Return;

Else
Begin
    Declare @var0 nvarchar(128);
    Select @var0 = name
    From sys.default_constraints
    Where parent_object_id = object_id(N'dbo.TenantAlias')
    AND col_name(parent_object_id, parent_column_id) = 'SignupPolicy';

    IF @var0 Is Not Null
        Execute('Alter Table [dbo].[TenantAlias] DROP CONSTRAINT [' + @var0 + ']');

    Alter Table [dbo].[TenantAlias] Drop Column [SignupPolicy]

    Declare @var1 nvarchar(128);
    Select @var1 = name
    From sys.default_constraints
    Where parent_object_id = object_id(N'dbo.TenantAlias')
    AND col_name(parent_object_id, parent_column_id) = 'SigninPolicy';

    IF @var1 Is Not Null
        Execute('Alter Table [dbo].[TenantAlias] DROP CONSTRAINT [' + @var1 + ']');

    Alter Table [dbo].[TenantAlias] Drop Column [SigninPolicy];

    Declare @var2 nvarchar(128);
    Select @var2 = name
    From sys.default_constraints
    Where parent_object_id = object_id(N'dbo.TenantAlias')
    AND col_name(parent_object_id, parent_column_id) = 'ProfilePolicy';

    IF @var2 Is Not Null
        Execute('Alter Table [dbo].[TenantAlias] DROP CONSTRAINT [' + @var2 + ']');

    Alter Table [dbo].[TenantAlias] Drop Column [ProfilePolicy];

    Declare @var3 nvarchar(128);
    Select @var3 = name
    From sys.default_constraints
    Where parent_object_id = object_id(N'dbo.TenantAlias')
    AND col_name(parent_object_id, parent_column_id) = 'TenantPath';

    IF @var3 Is Not Null
        Execute('Alter Table [dbo].[TenantAlias] DROP CONSTRAINT [' + @var3 + ']');

    Alter Table [dbo].[TenantAlias] Drop Column [TenantPath];

    Delete [dbo].[__MigrationHistory]
    Where (([MigrationId] = N'201908130014246_4-PolicyInfo') AND ([ContextKey] = N'Masticore.Infrastructure.Azure.Migrations.Configuration'))

End
Go