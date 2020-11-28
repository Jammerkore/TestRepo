alter table "OVERRIDE_LL_MODEL_DETAIL"
   add constraint "OVERRIDE_LL_DET_VERSION_RID_FK1" foreign key (
       "VERSION_RID")
    references "FORECAST_VERSION" (
       "FV_RID")
go
