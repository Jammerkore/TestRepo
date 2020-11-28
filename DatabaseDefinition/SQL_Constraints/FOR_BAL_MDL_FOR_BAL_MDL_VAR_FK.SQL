/* Add foreign key constraints to table "FORECAST_BAL_MODEL_VARIABLE".                        */
alter table "FORECAST_BAL_MODEL_VARIABLE"
	add constraint "FOR_BAL_MDL_FOR_BAL_MDL_VAR_FK" foreign key (
		"FBMOD_RID")
	 references "FORECAST_BAL_MODEL" (
		"FBMOD_RID")  

go
