Public MustInherit Class KeyCode
    Dim KeyCode As Integer

    Public Function write(kc As Integer) As Boolean
        KeyCode = kc

        ' Return Successfully
        Return True
    End Function

    Public Function read() As Integer
        Return KeyCode
    End Function

    Public MustOverride Function toFreq() As Integer
    Public MustOverride Function toKeyCode(Freq As Integer) As Integer
End Class
