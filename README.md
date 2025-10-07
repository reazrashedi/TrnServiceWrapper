# TerminalInquiryLib (Class Library scaffold)

این پروژه یک scaffold برای ساخت **Class Library** است که به عنوان wrapper برای WCF client (TRNService) عمل می‌کند.
هدف: تولید یک DLL که بتوان از طریق Python (با pythonnet) یا دیگر پلتفرم‌ها آن را فراخوانی کرد.

---

## مراحل سریع (Windows)

1. باز کردن پوشه پروژه در Visual Studio یا از خط فرمان:
```powershell
cd TerminalInquiryLib
dotnet restore
```

2. تولید proxy از WSDL (نیازمند dotnet-svcutil):
```powershell
# اگر dotnet-svcutil نصب نشده:
dotnet tool install --global dotnet-svcutil

# سپس اجرای اسکریپت:
.\GenerateProxy.ps1
```

این اسکریپت تلاش می‌کند فایل `ConnectedServices\TRNService.cs` را تولید کند. بعد از تولید:
- فایل `ConnectedServices\TRNService.cs` را به پروژه اضافه کنید (در همان namespace).
- Rebuild کنید. سپس wrapper قادر خواهد بود تا proxy تولید‌شده را بارگذاری و متدها را فراخوانی کند.

> توجه: اگر سرویس از binding یا security خاصی (مانند wsHttpBinding یا certificate) استفاده می‌کند، ممکن است لازم باشد تنظیمات binding در پروژه را شخصی‌سازی کنید.

---

## استفاده از DLL در Python (نمونه)

ابتدا DLL را بسازید (Build -> Release). سپس در پایتون:

```python
import clr
clr.AddReference(r"C:\path\to\TerminalInquiryLib\bin\Debug\net6.0\TerminalInquiryLib.dll")

from TerminalInquiryLib import TerminalServiceClientWrapper

client = TerminalServiceClientWrapper("https://bar.rmto.ir/Service2/TRNServiceV02.svc")
res = client.GetDriverByNational_ID("1234567890")
print(res)
```

برای اجرای از pythonnet:
```
pip install pythonnet
```

---

## نکات و هشدارها

- اگر generation با dotnet-svcutil موفق نشد، به دلیل محدودیت‌های binding یا security ممکن است نیاز به تولید proxy با گزینه‌های دستی یا استفاده از svcutil.exe در Windows باشد.
- این scaffold از reflection برای پیدا کردن کلاس proxy استفاده می‌کند؛ بنابراین نام namespace و کلاس تولیدی باید مطابق اسکریپت باشد (`TerminalInquiryLib.ConnectedServices.TRNService.TRNServiceClient` یا مشابه).
- اگر سرویس از احراز هویت یا گواهی استفاده می‌کند، باید قبل از فراخوانی از داخل پایتون، تنظیمات مناسب (certificate validation, credentials) را اعمال کنید.

---

## فایل‌های پروژه
- TerminalInquiryLib.csproj
- src/TerminalServiceClientWrapper.cs
- GenerateProxy.ps1
- README.md
- sample_use_python.py
