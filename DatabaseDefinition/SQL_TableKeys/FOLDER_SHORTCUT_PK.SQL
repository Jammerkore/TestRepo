alter table "FOLDER_SHORTCUT"
	add constraint "FOLDER_SHORTCUT_PK" primary key clustered ("PARENT_FOLDER_RID", "CHILD_SHORTCUT_RID", "CHILD_SHORTCUT_TYPE")   



GO
