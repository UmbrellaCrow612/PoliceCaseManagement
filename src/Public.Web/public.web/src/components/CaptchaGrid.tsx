'use client'

import { useEffect, useState } from 'react'
import Image from 'next/image'
import { Button } from '@/components/ui/button'
import { Skeleton } from '@/components/ui/skeleton'
import { AlertCircle, RefreshCw } from 'lucide-react'
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert'

interface CaptchaChild {
  id: string
  bytes: string
}

interface CaptchaData {
  id: string
  text : string
  children: CaptchaChild[]
}

export default function CaptchaGrid() {
  const [data, setData] = useState<CaptchaData | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  const fetchData = async () => {
    setIsLoading(true)
    setError(null)
    try {
      const response = await fetch('https://localhost:7052/captcha/grid')
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }
      const json = await response.json()
      setData(json)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An unknown error occurred')
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    fetchData()
  }, [])

  if (error) {
    return (
      <Alert variant="destructive">
        <AlertCircle className="h-4 w-4" />
        <AlertTitle>Error</AlertTitle>
        <AlertDescription>{error}</AlertDescription>
      </Alert>
    )
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Captcha Grid</h1>
        <Button onClick={fetchData} disabled={isLoading}>
          <RefreshCw className="mr-2 h-4 w-4" />
          Refresh
        </Button>
      </div>
      {isLoading ? (
        <div className="grid grid-cols-3 gap-4">
          {[...Array(9)].map((_, index) => (
            <Skeleton key={index} className="h-32 w-full" />
          ))}
        </div>
      ) : data ? (
        <>
          <p className="mb-4">Parent ID: {data.id} Value you need to make by picking selected children: {data.text}</p>
          <div className="grid grid-cols-3 gap-4">
            {data.children.map((child) => (
              <div key={child.id} className="relative h-32">
                <Image
                  src={`data:image/png;base64,${child.bytes}`}
                  alt={`Captcha image ${child.id}`}
                  fill
                  className="object-cover rounded-md"
                />
              </div>
            ))}
          </div>
        </>
      ) : null}
    </div>
  )
}

