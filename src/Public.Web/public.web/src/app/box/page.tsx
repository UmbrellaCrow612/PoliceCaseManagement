import CaptchaBox from "@/components/CaptchaBox";
import Script from "next/script";

export default function Home() {
  return (
    <>
     <main className="flex min-h-screen items-center justify-center bg-gradient-to-r from-gray-100 to-gray-200">
      <CaptchaBox />
    </main>
    <Script src="/scripts/captcha.js" strategy="beforeInteractive" />
    </>
   
  )
}

