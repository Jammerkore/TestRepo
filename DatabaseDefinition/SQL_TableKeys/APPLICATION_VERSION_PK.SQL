alter table "APPLICATION_VERSION"
	add constraint "APPLICATION_VERSION_PK" primary key clustered ("MAJOR_VERSION", "MINOR_VERSION", "REVISION", "MODIFICATION")   



GO
