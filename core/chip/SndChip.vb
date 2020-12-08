Namespace Core
    Public Class SndChip
        ' Components
        Protected Reg As Memory
    End Class

    Namespace PSG
        Public Class PSGChip : Inherits SndChip

        End Class
    End Namespace

    Namespace FM
        Public Class FMChip : Inherits SndChip

            ' Additional PSG Chip Pointer
            Protected lpPSGChip As Long
            ' DAC Chip Pointer


        End Class
    End Namespace
End Namespace
