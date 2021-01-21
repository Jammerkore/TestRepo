--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads rows from the method
--dv =============================================
CREATE VIEW [dbo].[VW_METHOD_READ]
AS

	SELECT 
		m.METHOD_RID, 
		m.METHOD_NAME, 
		m.PROFILE_TYPE_ID, 
		m.METHOD_TYPE_ID, 
		ui.USER_RID,
		m.USER_RID as METHOD_USER,
		m.METHOD_DESCRIPTION, 
		m.SG_RID, 
		m.VIRTUAL_IND, 
		m.METHOD_STATUS, 
		m.CUSTOM_OLL_RID,
		ui.ITEM_TYPE, 
		ui.OWNER_USER_RID,
		coalesce(m.TEMPLATE_IND, '1') as TEMPLATE_IND
    FROM METHOD m
	INNER JOIN USER_ITEM ui WITH (NOLOCK) ON ui.ITEM_RID = m.METHOD_RID AND ui.ITEM_TYPE = m.PROFILE_TYPE_ID

GO
