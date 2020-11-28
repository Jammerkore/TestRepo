/*  BEGIN MID Track #5647 - KJohnson - Matrix Forecast */
alter table "METHOD_MATRIX"
	add constraint "FRCST_BAL_MDL_MTHD_MTRX_FK1" foreign key (
		"MODEL_RID")
	 references "FORECAST_BAL_MODEL" (
		"FBMOD_RID")  

go
