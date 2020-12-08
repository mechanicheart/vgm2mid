Namespace Core
    Public MustInherit Class IChip
        Protected iMastClock As Integer
        Protected iSlavClock As Integer

        Public MustOverride Sub Init()
    End Class
End Namespace