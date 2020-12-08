Imports VGM2MID.Instrument

' OPN Class
' + Inherited From FMChip Class
' + Components
'   - Memory (Registers, RAM or ROM)
'   - Memory Mapper (Register Port Mapper)
'   - Channels (Consists of Operators, Attributes)
Namespace Core
    Namespace SndChip
        Namespace OPN
            ' Generic OPNx Series
            Public MustInherit Class OPNxChip : Inherits FMChip
                Public Sub New()
                    iOperatorCount = 4
                End Sub

            End Class

            ' YM2203
            Public Class OPNChip : Inherits OPNxChip

                Public Overrides Sub Init()
                    Reg.BindMapper(New MMap.OPN.OPNMapper)
                End Sub
            End Class

            ' YM2608
            Public Class OPNAChip : Inherits OPNxChip
                Private ADPCMRom As Memory
                Public Overrides Sub Init()

                End Sub

            End Class

            ' YM2610
            Public Class OPNBChip : Inherits OPNxChip
                Private ADPCMRomA As Memory
                Private ADPCMRomB As Memory
                Public Overrides Sub Init()

                End Sub
            End Class

            Public Class OPN2Chip : Inherits OPNxChip
                Public Overrides Sub Init()

                End Sub
            End Class

        End Namespace
    End Namespace
End Namespace