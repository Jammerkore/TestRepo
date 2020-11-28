/* Add foreign key constraints to table "SIMILAR_STORES".                                     */
alter table "SIMILAR_STORES"
	add constraint "STORES_SIMILAR_STORES_FK2" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  
go
