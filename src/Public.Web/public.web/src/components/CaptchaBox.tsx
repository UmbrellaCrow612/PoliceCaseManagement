'use client'

import { useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Checkbox } from '@/components/ui/checkbox'
import { Label } from '@/components/ui/label'

export default function CaptchaBox() {
  const [isChecked, setIsChecked] = useState(false)

  const handleCheck = () => {
    setIsChecked(!isChecked)
    // Here you would typically verify the captcha
    console.log('Captcha checked:', !isChecked)
  }

  return (
    <Card className="w-[300px] shadow-lg">
      <CardHeader>
        <CardTitle className="text-blue-600 text-center">Captcha</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="flex items-center space-x-2">
          <Checkbox id="captcha" captcha-checked={isChecked.toString()} checked={isChecked} onCheckedChange={handleCheck} />
          <Label
            htmlFor="captcha"
            className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
          >
            I'm not a robot
          </Label>
        </div>
      </CardContent>
    </Card>
  )
}

