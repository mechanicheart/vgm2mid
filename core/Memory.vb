Imports VGM2MID.Util

Namespace Core
    Public Class Memory
        ' Main Storage
        ' Address: 32-Bits (Integer)
        ' Data: 8-Bits (Byte)
        Private bMem() As Byte

        ' ----------------------------------------------------------------------
        ' Public Properties
        Public Property Memory(addr As Integer) As Byte
            Get
                Return bMem(addr)
            End Get
            Set(val As Byte)
                bMem(addr) = val
            End Set
        End Property

        '-----------------------------------------------------------------------
        ' Public Methods
        Public Sub New()
            ' 256 Bytes
            ReDim Preserve bMem(255)
        End Sub

        Public Sub New(size As Integer)
            ReDim Preserve bMem(size - 1)
        End Sub

        Public Sub Alloc(size As Integer)
            ReDim Preserve bMem(size - 1)
        End Sub

        Public Function Write(addr As Integer, data As Byte) As Byte
            Dim res As Integer = bMem(addr)
            bMem(addr) = data
            Return res
        End Function

        Public Function Read(addr As Integer) As Byte
            Return bMem(addr)
        End Function

    End Class
End Namespace
