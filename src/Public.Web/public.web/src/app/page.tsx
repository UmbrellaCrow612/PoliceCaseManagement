import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  PhoneCall,
  Mail,
  MapPin,
  Shield,
  Users,
  Bell,
  FileText,
  Calendar,
} from "lucide-react";

export default function HomePage() {
  return (
    <div className="flex flex-col min-h-screen">
      <header className="bg-blue-900 text-white">
        <div className="container mx-auto px-4 py-6 flex flex-col sm:flex-row justify-between items-center">
          <div className="flex items-center space-x-2 mb-4 sm:mb-0">
            <Shield className="h-8 w-8" />
            <span className="text-2xl font-bold">City Police Department</span>
          </div>
          <nav>
            <ul className="flex flex-wrap justify-center sm:space-x-4">
              <li className="w-1/2 sm:w-auto text-center mb-2 sm:mb-0">
                <Link href="#about" className="hover:underline">
                  About
                </Link>
              </li>
              <li className="w-1/2 sm:w-auto text-center mb-2 sm:mb-0">
                <Link href="#services" className="hover:underline">
                  Services
                </Link>
              </li>
              <li className="w-1/2 sm:w-auto text-center mb-2 sm:mb-0">
                <Link href="#community" className="hover:underline">
                  Community
                </Link>
              </li>
              <li className="w-1/2 sm:w-auto text-center">
                <Link href="#contact" className="hover:underline">
                  Contact
                </Link>
              </li>
            </ul>
          </nav>
        </div>
      </header>

      <main className="flex-grow">
        <section className="bg-blue-800 text-white py-12 sm:py-20">
          <div className="container mx-auto px-4 text-center">
            <h1 className="text-3xl sm:text-4xl font-bold mb-4">
              Serving and Protecting Our Community
            </h1>
            <p className="text-lg sm:text-xl mb-8">24/7 Emergency: Call 911</p>
            <Link href="/report" className="w-full sm:w-auto px-3 py-2 border">
              Report a Non-Emergency
            </Link>
          </div>
        </section>

        <section id="about" className="py-12 sm:py-16">
          <div className="container mx-auto px-4">
            <h2 className="text-2xl sm:text-3xl font-bold mb-6 sm:mb-8 text-center">
              About Our Department
            </h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6 sm:gap-8">
              <Card>
                <CardHeader>
                  <CardTitle>Our Mission</CardTitle>
                </CardHeader>
                <CardContent>
                  <p>
                    To serve our community with integrity, respect, and
                    professionalism, ensuring the safety and well-being of all
                    citizens.
                  </p>
                </CardContent>
              </Card>
              <Card>
                <CardHeader>
                  <CardTitle>Our Values</CardTitle>
                </CardHeader>
                <CardContent>
                  <ul className="list-disc list-inside">
                    <li>Integrity</li>
                    <li>Respect</li>
                    <li>Accountability</li>
                    <li>Excellence</li>
                    <li>Community Partnership</li>
                  </ul>
                </CardContent>
              </Card>
            </div>
          </div>
        </section>

        <section id="services" className="bg-gray-100 py-12 sm:py-16">
          <div className="container mx-auto px-4">
            <h2 className="text-2xl sm:text-3xl font-bold mb-6 sm:mb-8 text-center">
              Our Services
            </h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 sm:gap-8">
              <Card>
                <CardHeader>
                  <Users className="h-8 w-8 mb-2" />
                  <CardTitle>Community Policing</CardTitle>
                </CardHeader>
                <CardContent>
                  <p>
                    Engaging with the community to build trust and solve
                    problems together.
                  </p>
                </CardContent>
              </Card>
              <Card>
                <CardHeader>
                  <Bell className="h-8 w-8 mb-2" />
                  <CardTitle>Emergency Response</CardTitle>
                </CardHeader>
                <CardContent>
                  <p>
                    Rapid response to emergencies and critical incidents to
                    ensure public safety.
                  </p>
                </CardContent>
              </Card>
              <Card>
                <CardHeader>
                  <FileText className="h-8 w-8 mb-2" />
                  <CardTitle>Crime Prevention</CardTitle>
                </CardHeader>
                <CardContent>
                  <p>
                    Proactive measures and education to prevent crime in our
                    community.
                  </p>
                </CardContent>
              </Card>
            </div>
          </div>
        </section>

        <section id="community" className="py-12 sm:py-16">
          <div className="container mx-auto px-4">
            <h2 className="text-2xl sm:text-3xl font-bold mb-6 sm:mb-8 text-center">
              Community Engagement
            </h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6 sm:gap-8">
              <Card>
                <CardHeader>
                  <CardTitle>Upcoming Events</CardTitle>
                </CardHeader>
                <CardContent>
                  <ul className="space-y-2">
                    <li className="flex items-center">
                      <Calendar className="h-5 w-5 mr-2 flex-shrink-0" />
                      <span>Community Safety Workshop - June 15</span>
                    </li>
                    <li className="flex items-center">
                      <Calendar className="h-5 w-5 mr-2 flex-shrink-0" />
                      <span>National Night Out - August 3</span>
                    </li>
                    <li className="flex items-center">
                      <Calendar className="h-5 w-5 mr-2 flex-shrink-0" />
                      <span>Youth Police Academy - July 10-14</span>
                    </li>
                  </ul>
                </CardContent>
              </Card>
              <Card>
                <CardHeader>
                  <CardTitle>Volunteer Opportunities</CardTitle>
                </CardHeader>
                <CardContent>
                  <p className="mb-4">
                    Get involved and make a difference in your community:
                  </p>
                  <Button className="w-full sm:w-auto">
                    Learn More About Volunteering
                  </Button>
                </CardContent>
              </Card>
            </div>
          </div>
        </section>

        <section id="contact" className="bg-blue-900 text-white py-12 sm:py-16">
          <div className="container mx-auto px-4">
            <h2 className="text-2xl sm:text-3xl font-bold mb-6 sm:mb-8 text-center">
              Contact Us
            </h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
              <div className="flex flex-col items-center text-center">
                <PhoneCall className="h-12 w-12 mb-4" />
                <h3 className="text-xl font-semibold mb-2">Phone</h3>
                <p>Emergency: 911</p>
                <p>Non-Emergency: (555) 123-4567</p>
              </div>
              <div className="flex flex-col items-center text-center">
                <Mail className="h-12 w-12 mb-4" />
                <h3 className="text-xl font-semibold mb-2">Email</h3>
                <p>info@citypolicedept.gov</p>
              </div>
              <div className="flex flex-col items-center text-center">
                <MapPin className="h-12 w-12 mb-4" />
                <h3 className="text-xl font-semibold mb-2">Address</h3>
                <p>123 Main Street</p>
                <p>City, State 12345</p>
              </div>
            </div>
          </div>
        </section>
      </main>

      <footer className="bg-gray-800 text-white py-8">
        <div className="container mx-auto px-4 text-center">
          <p>&copy; 2023 City Police Department. All rights reserved.</p>
          <div className="mt-4 space-y-2 sm:space-y-0">
            <Link
              href="/privacy"
              className="hover:underline block sm:inline-block sm:mr-4"
            >
              Privacy Policy
            </Link>
            <Link
              href="/accessibility"
              className="hover:underline block sm:inline-block"
            >
              Accessibility
            </Link>
          </div>
        </div>
      </footer>
    </div>
  );
}
