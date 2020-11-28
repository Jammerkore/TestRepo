/*RELEASE 17.04.0.02 8-23-2017 TT#2123-MD - JSmith - PH showing in OTS Forecast Reveiw Multi Level down to Size */

DECLARE @affectedRows INT = 1

WHILE @affectedRows > 0
BEGIN
	update HIERARCHY_NODE
	SET VIRTUAL_IND = '1'
	WHERE HN_RID in (SELECT HN_RID FROM HIER_NODE_JOIN hnj WHERE hnj.PARENT_HN_RID in (SELECT HN_RID FROM HIERARCHY_NODE hn WHERE hn.VIRTUAL_IND = '1'))
	  and VIRTUAL_IND <> '1'

	SET @affectedRows = @@ROWCOUNT
END

GO
