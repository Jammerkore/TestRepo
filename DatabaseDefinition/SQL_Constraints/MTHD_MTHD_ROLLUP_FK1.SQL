/* Add foreign key constraints to table "METHOD_ROLLUP".                               */
alter table "METHOD_ROLLUP"
	add constraint "MTHD_MTHD_ROLLUP_FK1" foreign key ("METHOD_RID")
	 references "METHOD" ("METHOD_RID") on update no action on delete no action  

go
