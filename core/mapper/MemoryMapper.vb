Imports VGM2MID.Util

Namespace Core
    ' Used For Memory/Port Mapping
    Public MustInherit Class MemoryMapper
        Protected Mapper(HashUtil.Size()) As Integer

        Public Sub New()
            MemoryUtil.MemSet(Mapper, -1, Mapper.Length)
        End Sub

        Public MustOverride Sub Init()

        ' ID (Handle) Bind To Memory Address
        Protected Function Bind(hID As Long, addr As Integer) As Boolean
            Return ArrayUtil.ArrSet(Mapper, hID, addr)
        End Function
        Protected Function Bind(hIDs As Long(), addrs As Integer()) As Boolean
            Return ArrayUtil.ArrSet(Mapper, hIDs, addrs)
        End Function
        ' String Bind To Memory Address
        Protected Function Bind(szStr As String, addr As Integer) As Boolean
            Return ArrayUtil.ArrSet(Mapper, HashUtil.Digest(szStr), addr)
        End Function
        Protected Function Bind(szStrs As String(), addrs As Integer()) As Boolean
            Return ArrayUtil.ArrSet(Mapper, ArrayUtil.ArrFunc(szStrs, AddressOf HashUtil.Digest), addrs)
        End Function

        ' ID (Handle) -> Memory Address
        Protected Function Map(hID As Long) As Integer
            Return Mapper(hID)
        End Function
        ' String -> Memory Address
        Protected Function Map(szStr As String) As Integer
            Return Mapper(HashUtil.Digest(szStr))
        End Function
    End Class
End Namespace
