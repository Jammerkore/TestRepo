/* Add foreign key constraints to table "METHOD_SIZE_BASIS_SUBSTITUTES".                      */
alter table "METHOD_SIZE_BASIS_SUBSTITUTES"
	add constraint "MT_SZ_BA_AL_MD_SZ_BA_SUBS_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_SIZE_BASIS_ALLOCATION" (
		"METHOD_RID")  

go
