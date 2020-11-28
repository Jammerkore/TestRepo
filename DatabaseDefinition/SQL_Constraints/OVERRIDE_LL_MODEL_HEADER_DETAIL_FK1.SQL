alter table "OVERRIDE_LL_MODEL_DETAIL"
   add constraint "OVERRIDE_LL_MODEL_HEADER_DETAIL_FK1" foreign key (
       "OLL_RID")
    references "OVERRIDE_LL_MODEL_HEADER" (
       "OLL_RID")
go
