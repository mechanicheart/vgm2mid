Namespace Util
    ' Used For String Mapping (String -> Long)
    Public Class HashUtil
        Public Enum HashType
            BKDRHash ' Default

            ' Currently Not Supported
            ' SDBMHash
            ' RSHash
            ' APHash
            ' ELFHash
            ' JSHash
            ' DEKHash
            ' FNVHash
            ' DJBHash
            ' DJB2Hash
            ' PJWHash
            ' DoubleHash
        End Enum

        Shared iHashType As HashType = HashType.BKDRHash
        Shared iHashSeed As Long = &H0
        Shared iHashSize As Long = &H1000

        Shared Property Type() As Integer
            Get
                Return iHashType
            End Get
            Set(val As Integer)
                iHashType = val
            End Set
        End Property

        Shared Property Seed() As Long
            Get
                Return iHashSeed
            End Get
            Set(val As Long)
                iHashSeed = val
            End Set
        End Property

        Shared Property Size() As Long
            Get
                Return iHashSize
            End Get
            Set(val As Long)
                iHashSize = val
            End Set
        End Property

        Shared Function Digest(str As String) As Long
            Dim iHash As Long = 0

            Return iHash
        End Function
    End Class
End Namespace
