Imports VGM2MID.Core

' OPN Class
' + Inherited From FMChip Class
' + Components
'   - Memory (Registers, RAM or ROM)
'   - Memory Mapper (Register Port Mapper)
'   - Channels (Consists of Operators, Attributes)

Namespace Core
    Namespace FM
        Namespace OPN
            ' Generic OPNx Series
            Public MustInherit Class OPNxChip : Inherits FM.FMChip

            End Class

            ' YM2203
            Public Class OPNChip : Inherits OPNxChip
                Private RegMap As OPNMapper
            End Class

            ' YM2608
            Public Class OPNAChip : Inherits OPNxChip
                Private RegMap As OPNAMapper

                Private ADPCMRom As Memory
            End Class

            ' YM2610
            Public Class OPNBChip : Inherits OPNxChip
                Private RegMap As OPNBMapper

                Private ADPCMRomA As Memory
                Private ADPCMRomB As Memory
            End Class

            Public Class OPN2Chip : Inherits OPNxChip
                Private RegMap As OPN2Mapper
            End Class

        End Namespace
    End Namespace
End Namespace