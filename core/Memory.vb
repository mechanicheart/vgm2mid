Namespace Core
    Public Class Memory
        ' Main Storage
        Private mem() As Byte

        Public Sub New()
            ' 256 Bytes
            alloc(&H100)
        End Sub

        Public Sub New(size As Integer)
            alloc(size)
        End Sub

        Public Sub alloc(size As Integer)
            ReDim Preserve mem(size - 1)
        End Sub

        Public Function write(addr As Integer, data As Byte) As Byte
            Dim res As Integer = mem(addr)
            mem(addr) = data
            Return res
        End Function

        Public Function read(addr As Integer) As Byte
            Return mem(addr)
        End Function
    End Class
End Namespace
