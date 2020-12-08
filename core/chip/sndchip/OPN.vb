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
                Public Sub New()
                    Reg = New Memory()
                    iOperatorCount = 4 : iChannelCount = 3 : fPortSupport = False
                    Reg.BindMapper(New MMap.OPN.OPNMapper())
                End Sub

                Public Sub New(clk As Integer)
                    Reg = New Memory()
                    iMClock = clk : iOperatorCount = 4 : iChannelCount = 3 : fPortSupport = False
                    Reg.BindMapper(New MMap.OPN.OPNMapper())
                End Sub

            End Class

            ' YM2608
            Public Class OPNAChip : Inherits OPNxChip
                Private ADPCMRom As Memory

                Public Sub New(clk As Integer)
                    iMClock = clk : iOperatorCount = 4 : iChannelCount = 6 : fPortSupport = True
                End Sub
            End Class

            ' YM2610
            Public Class OPNBChip : Inherits OPNxChip
                Private ADPCMRomA As Memory
                Private ADPCMRomB As Memory

                Public Sub New(clk As Integer)
                    iMClock = clk : iOperatorCount = 4 : iChannelCount = 6 : fPortSupport = True
                End Sub
            End Class

            Public Class OPN2Chip : Inherits OPNxChip
            End Class

        End Namespace
    End Namespace
End Namespace