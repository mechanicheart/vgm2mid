Namespace Util
    Public Class ArrayUtil
        ' Inner Buffer
        Private iArrBuf As Integer()
        Private szArrBuf As String()

        Shared Function ArrSet(arr As Integer(), index As Long, data As Integer) As Boolean
            If ((arr Is Nothing) Or index > arr.Length) Then Return False

            arr(index) = data

            Return True
        End Function

        Shared Function ArrSet(arr As Integer(), index As Integer(), data As Integer()) As Boolean
            If ((arr Is Nothing) Or (index Is Nothing) Or (data Is Nothing) Or index.Length <> data.Length) Then Return False

            For i As Integer = 0 To index.Length
                arr(index(i)) = data(i)
            Next

            Return True
        End Function

        Shared Function ArrSet(arr As Integer(), index As Long(), data As Integer()) As Boolean
            If ((arr Is Nothing) Or (index Is Nothing) Or (data Is Nothing) Or index.Length <> data.Length) Then Return False

            For i As Integer = 0 To index.Length
                arr(index(i)) = data(i)
            Next

            Return True
        End Function

        Shared Function ArrFunc(arr As Integer(), func As Func(Of Integer, Integer)) As Integer()
            If ((arr Is Nothing) Or (func Is Nothing)) Then Return Nothing

            Dim farr(arr.Length) As Integer

            For i As Integer = 0 To arr.Length
                farr(i) = func(arr(i))
            Next

            Return farr
        End Function

        Shared Function ArrFunc(arr As String(), func As Func(Of String, Integer)) As Long()
            If ((arr Is Nothing) Or (func Is Nothing)) Then Return Nothing

            Dim farr(arr.Length) As Long

            For i As Integer = 0 To arr.Length
                farr(i) = func(arr(i))
            Next

            Return farr
        End Function
    End Class
End Namespace
