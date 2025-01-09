'use client'

import { useState, useEffect } from 'react'
import Image from 'next/image'
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

interface Choice {
  id: string
  bytes: string
}

interface Answer {
  id: string
  answer: string
  choices: Choice[]
}

interface CaptchaData {
  id: string
  choices: Answer[]
}

export default function CaptchaCarousel() {
  const [data, setData] = useState<CaptchaData | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [visibleChoices, setVisibleChoices] = useState<{ [key: string]: boolean }>({})

  useEffect(() => {
    fetch('https://localhost:7052/captcha/carousel-games')
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok')
        }
        return response.json()
      })
      .then(data => {
        setData(data)
        const initialVisibility = data.choices.reduce((acc, choice) => {
          acc[choice.id] = false
          return acc
        }, {})
        setVisibleChoices(initialVisibility)
      })
      .catch(error => {
        console.error('Error fetching data:', error)
        setError('Failed to fetch data')
      })
      .finally(() => setLoading(false))
  }, [])

  const toggleChoice = (id: string) => {
    setVisibleChoices(prev => ({ ...prev, [id]: !prev[id] }))
  }

  if (loading) return <div className="p-4">Loading...</div>
  if (error) return <div className="p-4 text-red-500">Error: {error}</div>
  if (!data) return <div className="p-4">No data available</div>

  return (
    <div className="p-4 space-y-6">
      <h1 className="text-2xl font-bold mb-4">Captcha Carousel</h1>
      <p className="mb-4">Captcha ID: {data.id}</p>
      {data.choices.map(answer => (
        <Card key={answer.id} className="mb-6">
          <CardHeader>
            <CardTitle className="flex justify-between items-center">
              <span>Answer: {answer.answer}</span>
              <Button onClick={() => toggleChoice(answer.id)}>
                {visibleChoices[answer.id] ? 'Hide' : 'Show'}
              </Button>
            </CardTitle>
            <p className="text-sm text-gray-500">ID: {answer.id}</p>
            <p className="text-sm text-gray-500">Sub-choices: {answer.choices.length}</p>
          </CardHeader>
          <CardContent>
            {visibleChoices[answer.id] && (
              <details open>
                <summary className="cursor-pointer mb-2">Choice Details</summary>
                <div className="grid grid-cols-2 gap-4">
                  {answer.choices.map(choice => (
                    <Card key={choice.id} className="p-2">
                      <p className="mb-2 text-sm">Choice ID: {choice.id}</p>
                      {choice.bytes ? (
                        <Image
                          src={`data:image/jpeg;base64,${choice.bytes}`}
                          alt={`Choice ${choice.id}`}
                          width={200}
                          height={200}
                          className="w-full h-auto"
                        />
                      ) : (
                        <p className="text-sm text-gray-500">No image available</p>
                      )}
                    </Card>
                  ))}
                </div>
              </details>
            )}
          </CardContent>
        </Card>
      ))}
    </div>
  )
}

