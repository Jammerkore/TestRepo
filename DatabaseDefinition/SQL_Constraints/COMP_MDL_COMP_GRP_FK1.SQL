/* Add foreign key constraints to table "COMPUTATION_GROUP".                                  */
alter table "COMPUTATION_GROUP"
	add constraint "COMP_MDL_COMP_GRP_FK1" foreign key (
		"COMP_MODEL_RID")
	 references "COMPUTATION_MODEL" (
		"COMP_MODEL_RID")  

go
