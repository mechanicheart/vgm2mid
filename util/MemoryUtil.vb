Namespace Util
    Public Class MemoryUtil
        Public Declare Function ObjPtr Lib "MSVBVM60" Alias "VarPtr" (var As Object) As Long
        Public Declare Function VarPtr Lib "MSVBVM60" (var As Integer) As Long

        Shared Function MemSet(arr As Integer(), data As Integer, size As Integer) As Boolean
            If (arr Is Nothing) Then Return False
            If (arr.Length < size) Then size = arr.Length

            For i As Integer = 0 To arr.Length
                arr(i) = data
            Next
            Return True
        End Function
    End Class
End Namespace