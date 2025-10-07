# sample_use_python.py
# Demonstrates loading the produced DLL with pythonnet
# pip install pythonnet

import clr
import os
p = os.path.join(os.path.dirname(__file__), "src", "..", "bin", "Debug", "net6.0", "TerminalInquiryLib.dll")
print("DLL path (example):", p)
# Before running, build the project and adjust path to the built DLL
# clr.AddReference(p)
# from TerminalInquiryLib import TerminalServiceClientWrapper
# cli = TerminalServiceClientWrapper("https://bar.rmto.ir/Service2/TRNServiceV02.svc")
# print(cli.GetDriverByNational_ID("1234567890"))
