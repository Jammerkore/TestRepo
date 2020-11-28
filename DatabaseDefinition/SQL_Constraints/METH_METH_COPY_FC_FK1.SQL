/* Add foreign key constraints to table "METHOD_COPY_FORECAST".                               */
alter table "METHOD_COPY_FORECAST"
	add constraint "METH_METH_COPY_FC_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
