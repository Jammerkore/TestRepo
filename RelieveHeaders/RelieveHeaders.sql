select distinct h.HDR_ID as HeaderID, BN_ID as ProductID
  from VW_GET_HEADERS h 
  join BASE_NODE bn on bn.HN_RID = h.STYLE_HNRID 
  left join VW_GET_HEADERS m on m.HDR_RID = h.HDR_GROUP_RID
  left outer join dbo.HEADER_CHAR_JOIN hcj2 on hcj2.HDR_RID = h.HDR_RID
  left outer join dbo.HEADER_CHAR hc2 on hc2.HC_RID = hcj2.HC_RID
  left outer join dbo.HEADER_CHAR_GROUP hcg2 on hcg2.HCG_RID = hc2.HCG_RID
 where (h.Released = 1 and h.ShippingComplete = 0 and h.MultiHeader = 0) -- headers
    or (m.Released = 1 and m.ShippingComplete = 0 and m.MultiHeader = 1) -- members of multi headers
   and ((hcg2.HCG_ID = 'DC Acknowledged' and hc2.TEXT_VALUE = 'Y')
    or DATEDIFF ( day , h.RELEASE_DATETIME, GETDATE() ) > 30)