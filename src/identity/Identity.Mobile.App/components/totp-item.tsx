import React, { useEffect, useState } from "react";
import { StyleSheet, Text, View } from "react-native";
import * as OTPAuth from "otpauth";

export interface TotpData {
  appName: string;
  userName: string;
  secret: string;
  issuer: string;
}

export default function TotpItem(props: { details: TotpData }) {
  const { details } = props;
  const [code, setCode] = useState<string>("------");
  const [timeLeft, setTimeLeft] = useState<number>(30);

  useEffect(() => {
    const totp = new OTPAuth.TOTP({
      issuer: details.issuer,
      label: details.userName,
      algorithm: "SHA1",
      digits: 6,
      period: 30,
      secret: details.secret,
    });

    const updateCode = () => {
      const now = Math.floor(Date.now() / 1000);
      setCode(totp.generate());
      setTimeLeft(30 - (now % 30));
    };

    updateCode();

    const interval = setInterval(() => {
      setTimeLeft((prev) => {
        if (prev === 1) {
          updateCode();
          return 30;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(interval);
  }, [details.secret]);

  return (
    <View style={styles.container}>
      <Text style={styles.title} numberOfLines={1}>
        {details.appName} : {details.userName}
      </Text>
      <View style={styles.next}>
        <Text style={styles.code}>{code}</Text>
        <Text style={styles.timer}>{timeLeft}s</Text>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    height: 120,
    backgroundColor: "white",
    padding: 16,
    borderRadius: 8,
    shadowColor: "rgba(0, 0, 0, 0.15)",
    shadowOffset: { width: 1.95, height: 1.95 },
    shadowOpacity: 1,
    shadowRadius: 2.6,
    elevation: 3,
  },
  title: {
    fontSize: 18,
    fontWeight: "500",
    overflow: "hidden",
  },
  next: {
    flex: 1,
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
  },
  code: {
    fontSize: 30,
    color: "#1cd5e6",
    fontWeight: "bold",
  },
  timer: {
    fontSize: 17,
    fontWeight: "bold",
    color: "#555",
  },
});
