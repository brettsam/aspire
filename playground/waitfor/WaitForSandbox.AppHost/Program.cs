// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

var builder = DistributedApplication.CreateBuilder(args);

var pg = builder.AddPostgres("pg")
                .AsAzurePostgresFlexibleServer()
                .WithPgAdmin();

var db = pg.AddDatabase("db");

var dbsetup = builder.AddProject<Projects.WaitForSandbox_DbSetup>("dbsetup")
                     .WithReference(db)
                     .WaitFor(pg);

builder.AddProject<Projects.WaitForSandbox_ApiService>("api")
       .WithExternalHttpEndpoints()
       .WaitForCompletion(dbsetup)
       .WithReference(db);

#if !SKIP_DASHBOARD_REFERENCE
// This project is only added in playground projects to support development/debugging
// of the dashboard. It is not required in end developer code. Comment out this code
// or build with `/p:SkipDashboardReference=true`, to test end developer
// dashboard launch experience, Refer to Directory.Build.props for the path to
// the dashboard binary (defaults to the Aspire.Dashboard bin output in the
// artifacts dir).
builder.AddProject<Projects.Aspire_Dashboard>(KnownResourceNames.AspireDashboard);
#endif

builder.Build().Run();
