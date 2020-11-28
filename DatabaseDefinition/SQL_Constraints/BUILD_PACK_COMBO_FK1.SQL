alter table "BUILD_PACK_CONFIG"
	add constraint "BUILD_PACK_COMBO_FK1" foreign key (
		"BPC_COMBO_RID")
	 references "BUILD_PACK_COMBO" (
		"BPC_COMBO_RID")  

GO
