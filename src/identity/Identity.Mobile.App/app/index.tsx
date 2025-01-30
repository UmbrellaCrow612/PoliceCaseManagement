import { View, StyleSheet } from "react-native";
import TotpItem from "@/components/totp-item";
import ScanBtn from "@/components/scan-btn";

export default function TotpListTab() {
  return (
    <View style={styles.container}>
      <ScanBtn />
      <TotpItem appName="PCMS" issuer="PCMS" secret="JBSWY3DPELPK3PXP" userName="you@ibicbwcwiciuwbcibcgmail.com" />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 24,
  },
});
